import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/HomeView.vue'),
      meta: { title: 'Kostas | ~' }
    },
    {
      path: '/lebenslauf',
      name: 'resume',
      component: () => import('@/views/ResumeView.vue'),
      meta: { title: 'Kostas | Lebenslauf' }
    },
    {
       path: '/projekte',
       name: 'projekte',
       component: () => import('@/views/ProjectsView.vue'), 
        meta: { title: 'Kostas | Projekte' }
    },
        {
       path: '/blog',
       name: 'All Blogs',
       component: () => import('@/views/BlogsView.vue'),
       meta: { title: 'Kostas | Blog' }
    },
    {
      path: '/auth/callback',
      name: 'Login Callback',
      component: () => import('@/views/AuthCallbackView.vue'),
    },
    {
      path: '/admin/editor',
      name: 'Editor',
      component: () => import ('@/views/EditorView.vue'),
      meta: { title: 'Kostas | Editor' }
    },
    {
      path: '/blog/:slug',
      name: 'BlogPost',
      component: () => import ('@/views/BlogPostView.vue'),
      meta: { title: 'Kostas | Blog-Post' }
    },
    {
      path: '/projekte/:slug',
      name: 'ProjectPost',
      component: () => import ('@/views/ProjectPostView.vue'),
      meta: { title: 'Kostas | Projekt-Post' }
    }, 
    {
      path: '/auth/callback',
      name: 'AuthCallback',
      component: () => import ('@/views/AuthCallbackView.vue'),
    }, 
    {
      path: '/guestbook',
      name: 'Guestbook',
      component: () => import ('@/views/GuestbookView.vue'),
      meta: { title: 'Kostas | Gästebuch' }
    }, 
    {
      path: '/terms-of-service',
      name: 'TermsOfService',
      component: () => import ('@/views/TermsOfService.vue'),
      meta: { title: 'Kostas | Nutzungsbedingungen' }
    }, 
    {
      path: '/privacy-policy',
      name: 'PrivacyPolicy',
      component: () => import ('@/views/PrivacyPolicy.vue'),
      meta: { title: 'Kostas | Datenschutzerklärung' }
    }, 
    {
      path: '/impressum',
      name: 'Impressum',
      component: () => import ('@/views/Impressum.vue'),
      meta: { title: 'Kostas | Impressum' }
    }, 

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

  router.afterEach((to) => {
    // Fallback
    document.title = to.meta.title as string || 'Dein Name - Portfolio'
  })

export default router
