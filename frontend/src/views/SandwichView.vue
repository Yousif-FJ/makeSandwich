<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useSandwichesStore } from '../stores/sandwiches'
import { useOrdersStore } from '../stores/orders';
import { useAuthStore } from '@/stores/auth';

const sandwichesStore = useSandwichesStore();
const ordersStore = useOrdersStore();
const authStore = useAuthStore();

onMounted(async () => {
  await sandwichesStore.fetchSandwiches();
})

const breadName = ref('');
const breadType = ref('');
</script>

<template>
  <div v-if="authStore.isLogged" class="my-2">
    <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#addModal">
      Add sandwich
    </button>

  </div>
  <div class="row">
    <div v-for="sandwich in sandwichesStore.Sandwiches" :key="sandwich.id" class="col-sm-3 ">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">{{ sandwich.name }}</h5>
          <p class="card-text"> Bread type: {{ sandwich.breadType }}</p>
          <p class="card-text"> Sandwich ID : {{ sandwich.id }}</p>
          <a href="#" @click="ordersStore.createOrder(sandwich.id)" class="btn btn-primary">Order</a>
        </div>
      </div>
    </div>
  </div>


  <div class="modal fade" id="addModal" data-bs-backdrop="static"  tabindex="-1" aria-labelledby="Modal" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="ModalLabel">Add sandwich</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <form id="create-sandwich" class="modal-body">
          <div class="mb-3">
            <label for="sandwich-name-input" class="form-label">Sandwich name</label>
            <input v-model="breadName" class="form-control" id="sandwich-name-input" aria-describedby="sandwich-name-input">
          </div>
          <div class="mb-3">
            <label for="bread-type-select" class="form-label">Bread type</label>
            <select v-model="breadType" id="bread-type-select" class="form-select">
              <option value="Oat">Oat</option>
              <option value="Wheat">Wheat</option>
              <option value="Rye">Rye</option>
            </select>
          </div>
        </form>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          <button type="submit" form="create-sandwich" class="btn btn-primary" 
          @click.prevent="sandwichesStore.addSandwich(breadName, breadType)">Add</button>
        </div>
      </div>
    </div>
  </div>

</template>
