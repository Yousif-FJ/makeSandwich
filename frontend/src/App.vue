<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import NotificationComp from './components/NotificationComp.vue';
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import { useAuthStore } from './stores/auth';

const authStore = useAuthStore();

</script>

<template>
  <header>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
      <div class="container-fluid">
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
          aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav">
            <li class="nav-item">
              <RouterLink class="nav-link" to="/">Sandwich</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink class="nav-link" to="/orders">Orders</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink class="nav-link" to="/about">About</RouterLink>
            </li>
          </ul>
          <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
            <RouterLink v-if="!authStore.isLogged" class="nav-link" to="/login">Login</RouterLink>
            <a v-if="authStore.isLogged" href="#" class="nav-link" @click="authStore.logout">Logout</a>
          </ul>
        </div>
      </div>
    </nav>
  </header>
  <NotificationComp></NotificationComp>
  <main class="container-fluid mt-1">
    <RouterView />
  </main>
</template>
