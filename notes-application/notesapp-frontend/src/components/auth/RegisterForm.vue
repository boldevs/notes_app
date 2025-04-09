<template>
  <div class="min-h-screen flex items-center justify-center px-4 bg-gray-100">
    <form @submit.prevent="handleSubmit" class="bg-white p-6 sm:p-8 rounded-lg shadow-lg w-full max-w-md space-y-6">
      <h2 class="text-2xl sm:text-3xl font-bold text-center text-gray-800">Register</h2>

      <!-- Username Input -->
      <el-input v-model="form.username" placeholder="Username" clearable
        :class="{ 'border border-red-500 rounded': usernameError }" class="w-full" />

      <!-- Password Input -->
      <el-input v-model="form.password" :type="passwordVisible ? 'text' : 'password'" placeholder="Password" clearable
        :class="{ 'border border-red-500 rounded': passwordError }" class="w-full">
        <template #suffix>
          <el-icon @click="togglePassword" class="cursor-pointer">
            <component :is="passwordVisible ? 'i-ep-view' : 'i-ep-view-off'" />
          </el-icon>
        </template>
      </el-input>

      <!-- Login Button -->
      <el-button type="primary" class="w-full" native-type="submit">
        Register
      </el-button>

    </form>
  </div>
</template>


<script setup lang="ts">
import { reactive, ref, computed } from 'vue';
import { useAuthStore } from '../../store/auth';
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus';
import 'element-plus/es/components/alert/style/css';


const auth = useAuthStore();

const router = useRouter()

const form = reactive({
  username: '',
  password: '',
});

const submitted = ref(false);
const passwordVisible = ref(false);
const loginSuccess = ref(false);

// Field-specific validation
const usernameError = computed(() => submitted.value && !form.username);
const passwordError = computed(() => submitted.value && !form.password);

const togglePassword = () => {
  passwordVisible.value = !passwordVisible.value
}

const handleSubmit = async () => {
  submitted.value = true;

  if (!form.username || !form.password) {
    return;
  }

  try {
    await auth.register({ ...form });

    if (auth.token) {
      loginSuccess.value = true;
      ElMessage.success('Register successfully!');
      router.push('/');
    } else if (auth.error) {
      ElMessage.error(`Register exception. ${auth.error}`);
    }

  } catch (err) {
    console.error('Register exception:', err);
    ElMessage.error('An unexpected error occurred. Please try again later.');
  }
};
</script>

<style scoped>
.el-input .el-input__inner {
  padding-left: 36px !important;
}
</style>
