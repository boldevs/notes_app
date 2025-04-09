import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../store/auth'
import Login from '../pages/Login.vue'
import Register from '../pages/Register.vue'
import NotePage from '../pages/NotePage.vue'
import LayoutContainer from '../components/layout/LayoutContainer.vue'
import NoteDetail from '../pages/NoteDetail.vue'

const routes = [
  {
    path: '/',
    redirect: '/notespage',
  },
  {
    path: '/login',
    component: Login,
    meta: { guestOnly: true },
  },
  {
    path: '/register',
    component: Register,
    meta: { guestOnly: true },
  },
  {
    path: '/',
    component: LayoutContainer,
    children: [
      {
        path: '/notespage',
        component: NotePage,
        meta: { requiresAuth: true },
      },
      {
        path: '/notes/:id',
        name: 'NoteDetail',
        component: NoteDetail,
        meta: { requiresAuth: true },
      }
    ]
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// Navigation guard
router.beforeEach((to, from, next) => {
  const auth = useAuthStore()

  const isLoggedIn = !!auth.token

  if (to.meta.requiresAuth && !isLoggedIn) {
    return next('/login')
  }

  if (to.meta.guestOnly && isLoggedIn) {
    return next('/notespage')
  }

  next()
})

export default router
