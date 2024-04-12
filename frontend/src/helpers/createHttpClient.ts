import axios from 'axios';

export default function createHttpClient() {
    return axios.create({
        baseURL: 'http://localhost:12345',
    })
}