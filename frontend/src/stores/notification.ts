import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('Notification', () => {
    const popup = ref(null as popup | null);
    const isLoading = ref(false);
    let timeOut: number | null = null;
    
    function showNotification(message: string, type: notificationType) : void {
        isLoading.value = false;
        popup.value = { message, type };
        if(timeOut) clearTimeout(timeOut);
         timeOut = window.setTimeout(() => {
            popup.value = null;
        }, 3000);
    }

    function showLoading(){
        isLoading.value = true;
    }

    function clearNotification(){
        isLoading.value = false;
        popup.value = null;
    }

    return { popup, isLoading, showNotification, showLoading, clearNotification }
});

type notificationType = "success" | "error";

type popup = {
    message: string;
    type: notificationType;
};
