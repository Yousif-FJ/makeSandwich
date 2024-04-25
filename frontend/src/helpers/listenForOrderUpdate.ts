import * as signalR from "@microsoft/signalr";
import { useOrdersStore } from "../stores/orders";
import { useNotificationStore } from "../stores/notification";

export default function listenForOrderUpdate(){
    const orderStore = useOrdersStore();    
    const notificationStore = useNotificationStore();
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:12345/v1/orderStatus")
        .build();
    
    connection.start().then(() => {
        notificationStore.showNotification("Connected to RTC hub!", "success");
        console.log("Connected to RTC hub!");
    });
    
    connection.onclose(() => {
        notificationStore.showNotification("Disconnected from RTC hub!", "error");
        console.log("Disconnected from RTC hub!");
    });
    
    connection.on("OrderStatusUpdated", () => {
        console.log("Order signal update");
        orderStore.fetchOrders();
    });
}
