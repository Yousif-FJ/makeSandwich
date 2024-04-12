import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import createHttpClient from '@/helpers/createHttpClient'

export const useSandwichesStore = defineStore('Sandwiches', () => {
  const Sandwiches = ref(null as Sandwich[] | null);

  async function fetchSandwiches() : Promise<void> {
    const axios = createHttpClient();
    const result = await axios.get('/v1/sandwich');
    Sandwiches.value = result.data;
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