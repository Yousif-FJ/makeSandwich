import { createRouter, createWebHistory } from 'vue-router'
import SandwichView from '../views/SandwichView.vue'
import OrdersView from '../views/OrdersView.vue'
import AboutView from '../views/AboutView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'sandwich',
      component: SandwichView
    },
    {
      path: '/orders',
      name: 'orders',
      component: OrdersView
    },
    {
      path: '/about',
      name: 'about',
      component: AboutView
    }
  ]
})

export default router
