<script setup lang="ts">
import { useOrdersStore } from '../stores/orders';
import { onMounted } from 'vue';
import listenForOrderUpdate from '../helpers/listenForOrderUpdate';

const ordersStore = useOrdersStore();

onMounted(async () => {
    listenForOrderUpdate();
})

</script>
<template>
    <h3>Order status</h3>
    <p>Status updates in automatically in real-time</p>
    <div class="row">
        <div v-for="order in ordersStore.orders" :key="order.id" class="col-sm-3 ">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">order-{{ order.id }}</h5>
                    <h6 class="card-subtitle mb-2 text-body-secondary">Order status: {{ order.status }}</h6>
                    <p class="card-text">Sandwich {{ order.sandwichId }}</p>
                </div>
            </div>
        </div>
    </div>
</template>