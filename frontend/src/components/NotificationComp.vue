<script setup lang="ts">
import { useNotificationStore } from '../stores/notification';
import { computed } from 'vue';

const notificationStore = useNotificationStore();


const notificationColorClass = computed(() => {
  if (notificationStore.popup?.type === 'error') {
    return 'text-bg-danger'
  } else {
    return 'text-bg-success'
  }
})

const notificationVisibilityCss = computed(() => {
  if (notificationStore.popup?.message) {
    return 'display: block !important'
  } else {
    return 'display: none !important'
  }
})

</script>
<template>
  <div class="toast-container position-fixed bottom-0 end-0 p-3">
    <div id="liveToast" role="alert" :class="`toast align-items-center ${notificationColorClass} border-0`"
      :style="notificationVisibilityCss" aria-live="assertive" aria-atomic="true">
      <div class="d-flex">
        <div class="toast-body">
          {{ notificationStore.popup?.message }}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto"
          @click="notificationStore.clearNotification()" aria-label="Close"></button>
      </div>
    </div>
  </div>

  <div v-if="notificationStore.isLoading" class="position-absolute top-50 start-50 z-3 translate-middle">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</template>