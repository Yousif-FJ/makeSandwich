<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import NotificationComp from './components/NotificationComp.vue';
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"

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
        </div>
      </div>
    </nav>
  </header>
  <NotificationComp></NotificationComp>
  <main class ="container-fluid mt-1">
    <RouterView />
  </main>
</template>
