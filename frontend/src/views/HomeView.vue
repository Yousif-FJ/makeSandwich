<script setup lang="ts">
import { onMounted } from 'vue';
import { useSandwichesStore } from '../stores/sandwiches'

const sandwichesStore = useSandwichesStore();

onMounted(async () => {
  await sandwichesStore.fetchSandwiches();
})

</script>

<template>
  <main class="mt-1">
    <div v-if="sandwichesStore.Sandwiches === null">Loading...</div>
    <div v-for="sandwich in sandwichesStore.Sandwiches" :key="sandwich.id"  class="card" style="width: 18rem;">
      <div class="card-body">
        <h5 class="card-title">{{ sandwich.name }}</h5>
        <h6 class="card-subtitle mb-2 text-body-secondary"></h6>
        <p class="card-text"> {{ sandwich.breadType  }}</p>
        <a href="#" class="card-link">Add to cart</a>
      </div>
    </div>
  </main>
</template>
