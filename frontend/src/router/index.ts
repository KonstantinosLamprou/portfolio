import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/HomeView.vue')
    },
    {
      path: '/lebenslauf',
      name: 'resume',
      component: () => import('@/views/ResumeView.vue')
    },
    {
       path: '/projekte',
       name: 'projekte',
       component: () => import('@/views/ProjectsView.vue')
    },
        {
       path: '/blog',
       name: 'All Blogs',
       component: () => import('@/views/AllBlogView.vue')
    },
    // Admin route deprecated
    {
    path: '/admin/login',
    name: 'Login Admin',
    component: () => import('@/views/LoginView.vue'),
    },
    {
      path: '/admin/editor',
      name: 'Editor',
      component: () => import ('@/views/EditorView.vue')
    },
    {
      path: '/blog/:slug',
      name: 'BlogPost',
      component: () => import ('@/views/BlogPostView.vue')
    },
    {
      path: '/projects/:slug',
      name: 'ProjectPost',
      component: () => import ('@/views/ProjectPostView.vue')
    }
  ],

  scrollBehavior(to, from, savedPosition) {
      // Wenn der User die Vor/Zurück-Tasten des Browsers nutzt,
      // springt er zur gemerkten Position zurück
      if (savedPosition) {
        return savedPosition
      }

      if (to.hash) {
        return {
          el: to.hash,           // Das Element mit der ID, z.B. '#block-1'
          behavior: 'smooth',
          top: 200
        }
      }
      // Bei jedem normalen Klick auf einen neuen Link geht es ganz nach oben

      return { top: 0 }

  }
})

export default router
