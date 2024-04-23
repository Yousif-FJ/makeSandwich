<script setup lang="ts">
import { onMounted } from 'vue';
import { useSandwichesStore } from '../stores/sandwiches'
import { useOrdersStore } from '../stores/orders';

const sandwichesStore = useSandwichesStore();
const ordersStore = useOrdersStore();

onMounted(async () => {
  await sandwichesStore.fetchSandwiches();
})

</script>

<template>
    <div class="row">
      <div v-for="sandwich in sandwichesStore.Sandwiches" :key="sandwich.id" class="col-sm-3 ">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ sandwich.name }}</h5>
            <p class="card-text"> Bread type: {{ sandwich.breadType }}</p>
            <a href="#" @click="ordersStore.createOrder(sandwich.id)" class="btn btn-primary">Order</a>
          </div>
        </div>
      </div>
    </div>
</template>
