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

  async function addSandwich(sandwichName: string, breadType: string) : Promise<void> {
    const notificationStore = useNotificationStore();
    const axios = createHttpClient();
    try{
      await axios.post('/v1/sandwich', {
        name: sandwichName,
        breadType: breadType,
      })
      notificationStore.showNotification('Added sandwich successfully', 'success');
      await fetchSandwiches();
    }
    catch(error){
      console.log(error);      
      notificationStore.showNotification('Failed to add sandwich', 'error');
    }
  }

  return { Sandwiches, fetchSandwiches, addSandwich }
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