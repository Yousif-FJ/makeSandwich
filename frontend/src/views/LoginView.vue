<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';

const authStore = useAuthStore();

const email = ref('');
const password = ref('');
</script>

<template>
    <h2>Login</h2>
    <form style="max-width: 500px;" @submit.prevent="authStore.login(email, password)">
        <div class="mb-3">
            <label for="InputEmail" class="form-label">Email address</label>
            <input v-model="email" type="email" class="form-control" id="InputEmail1">
        </div>
        <div class="mb-3">
            <label for="InputPassword1" class="form-label">Password</label>
            <input v-model="password" type="password" class="form-control" id="InputPassword1">
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
    </form>
</template>