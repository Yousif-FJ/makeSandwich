import { ref } from 'vue'
import { defineStore } from 'pinia'
import createHttpClient from '../helpers/createHttpClient'
import { useNotificationStore } from './notification';

export const useSandwichesStore = defineStore('Sandwiches', () => {
  const Sandwiches = ref(null as Sandwich[] | null);

  async function fetchSandwiches() : Promise<void> { 
    const notificationStore = useNotificationStore();

    const axios = createHttpClient();
    try{
      notificationStore.showLoading();
      const result = await axios.get('/v1/sandwich');
      Sandwiches.value = result.data;
      notificationStore.showNotification('Fetched sandwiches successfully', 'success');
    } catch(error){
      notificationStore.showNotification('Failed to fetch sandwiches', 'error');
    }
  }

  return { Sandwiches, fetchSandwiches }
});

export type Sandwich = {
    id: number;
    name: string;
    toppings: SandwichTopping[] | null;
    breadType: breadTypes;
};

export type SandwichTopping = {
    id: number;
    name: string;
};

export enum breadTypes {
    oat = 'Oat',
    rye = 'Rye',
    wheat = 'Wheat',
}