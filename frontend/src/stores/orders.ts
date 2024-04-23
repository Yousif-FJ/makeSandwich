import { defineStore } from 'pinia'
import { ref } from 'vue'
import createHttpClient from '../helpers/createHttpClient'
import { useNotificationStore } from './notification';


export const useOrdersStore = defineStore('Orders', () => {
    const orders = ref(null as Order[] | null);

    async function fetchOrders(): Promise<void> {
        const notificationStore = useNotificationStore();

        const axios = createHttpClient();
        try {
            notificationStore.showLoading();
            const response = await axios.get('/v1/order');
            orders.value = response.data;
            notificationStore.showNotification('Fetched orders successfully', 'success');
        } catch (error) {
            console.log(error);
            notificationStore.showNotification('Failed to fetch orders', 'error');
        }
    }

    async function createOrder(sandwichId: number): Promise<void> {
        const notificationStore = useNotificationStore();
        const axios = createHttpClient();
        const newOrder: Order = { sandwichId, status: orderStatus.ordered, id: 0 }
        try {
            await axios.post('/v1/order', newOrder);
            notificationStore.showNotification('Order created', 'success');
        } catch (error) {
            notificationStore.showNotification('Failed to create order', 'error');
        }
    }

    return { orders, fetchOrders, createOrder }
});

export type Order = {
    id: number;
    status: orderStatus;
    sandwichId: number;
};

export enum orderStatus {
    ordered = "Ordered",
    inQueue = "InQueue",
    received = "Received",
    ready = "Ready",
    failed = "Failed",
}
