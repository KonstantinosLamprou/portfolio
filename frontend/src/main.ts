import { createApp } from 'vue'
import { createPinia } from 'pinia'


import App from './App.vue'
import { VueQueryPlugin } from '@tanstack/vue-query'
import router from './router'
import './styles/styles.css'

const app = createApp(App)
const pinia = createPinia()
app.use(VueQueryPlugin)

app.use(pinia)
app.use(router)

app.mount('#app')
