import { defineStore } from 'pinia'
import createHttpClient from '../helpers/createHttpClient'
import { ref } from 'vue'
import { useNotificationStore } from './notification';
import router from '@/router';

export const useAuthStore = defineStore('Auth', () => {
    const isLogged = ref(false);

    const axios = createHttpClient();
    axios.get('/v1/auth').then((response) => {
        isLogged.value = response.status === 200;
    }).catch(() => { console.log("use is not logged in") }) ;

    async function login(email : string, password: string) {
        const notificationStore = useNotificationStore();
        const axios = createHttpClient();

        notificationStore.showLoading();
        const result = await axios.post('/v1/user/login', { username: '', email, password });
        if (result.status === 200) {
            isLogged.value = true;
            router.push('/');
            notificationStore.showNotification('Login successful', 'success');
        }else{
            notificationStore.showNotification('Login failed', 'error');
        }
    }

    async function logout() {
        const axios = createHttpClient();
        const notificationStore = useNotificationStore();

        notificationStore.showLoading();
        const result = await axios.post('/v1/user/logout');
        if (result.status === 200) {
            isLogged.value = false;
            notificationStore.showNotification('Logout successful', 'success');
        }else{
            notificationStore.showNotification('Logout failed', 'error');
        }
    }

    return { isLogged, login, logout }
});