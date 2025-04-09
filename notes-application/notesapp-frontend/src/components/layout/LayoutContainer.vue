<template>
  <el-container class="h-full">
    <!-- Sidebar -->
    <el-aside :width="collapsed ? '64px' : '220px'"
      class="transition-all duration-500 h-screen bg-slate-50 border-r border-gray-300">

      <!-- Logo/Header Section -->
      <div class="h-16 flex items-center justify-center border-b">
        <el-icon class="text-black" :style="{ fontSize: '22px' }">
          <Document />
        </el-icon>
        <span v-if="!collapsed" class="px-2 text-lg font-semibold">Note Application</span>
      </div>

      <!-- Sidebar Menu -->
      <el-menu :default-active="'/note'" class="my-4" :collapse="collapsed" background-color="#f8fafc"
        text-color="#1e293b" active-text-color="#2563eb" style="border-right: none;">
        <el-menu-item index="/note">
          <el-icon>
            <Eleme />
          </el-icon>
          <template #title>Default</template>
        </el-menu-item>
      </el-menu>
    </el-aside>

    <!-- Header + Main -->
    <el-container>
      <el-header class="my-1 md:mr-4 h-16 px-4 flex items-center justify-between">
        <div class="hidden md:block">
          <el-button @click="toggleSidebar" icon="Menu" circle class="text-black" />
        </div>
        <div class="w-full flex justify-end items-center">
          <el-dropdown trigger="click" @command="handleUserMenu">
            <div class="cursor-pointer flex items-center space-x-2">
              <el-icon :style="{ fontSize: '20px' }" class="text-black">
                <User />
              </el-icon>
            </div>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item disabled>
                  <el-icon>
                    <User />
                  </el-icon>
                  <span class="ml-1">{{ authStore.user || 'Guest' }}</span>
                </el-dropdown-item>
                <el-dropdown-item divided command="logout">
                  <el-icon>
                    <SwitchButton />
                  </el-icon>
                  <span class="ml-1">Logout</span>
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>

      </el-header>

      <el-main class=" md:mx-12 overflow-y-auto">
        <router-view />
      </el-main>

    </el-container>
  </el-container>
</template>

<script setup lang="ts">

import { onBeforeUnmount, onMounted, ref } from 'vue'
import { Eleme, Document, User, SwitchButton, Menu } from '@element-plus/icons-vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../store/auth'
import { ElMessage } from 'element-plus';

const router = useRouter()
const authStore = useAuthStore();

const handleUserMenu = (command: string) => {
  if (command === 'logout') {
    authStore.logout();
    ElMessage.success('You have logged out successfully');
    router.push('/login')
  }
}

const collapsed = ref(false)
const isMobile = ref(false)

const checkScreenSize = () => {
  isMobile.value = window.innerWidth < 768
  if (isMobile.value) {
    collapsed.value = true
  }
}

onMounted(() => {
  checkScreenSize()
  window.addEventListener('resize', checkScreenSize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', checkScreenSize)
})

const toggleSidebar = () => {
  if (!isMobile.value) {
    collapsed.value = !collapsed.value
  }
}


</script>
