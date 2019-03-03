import Vue from 'vue'
import App from './App.vue'
import 'bootstrap'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import './styles/FirePlatform.css'
import BootstrapVue from 'bootstrap-vue'
import { Collapse } from 'bootstrap-vue/es/components';
import Axios from 'axios';
import '../node_modules/nprogress/nprogress.css'
import VueProgressBar from 'vue-progressbar';

window.axios = require('axios');

Vue.use(VueProgressBar, {
  color: 'rgb(143, 255, 199)',
  failedColor: 'red',
  height: '10px', 
  transition: {
    speed: '1s',
    opacity: '2s',
    termination: 300
  },
})

Vue.use(BootstrapVue);
Vue.use(Collapse);

Vue.config.productionTip = false

Axios.defaults.baseURL = process.env.API_ENDPOINT;
export const serverBus = new Vue();

new Vue({
  render: h => h(App),
}).$mount('#app')

// Intercept all requests
window.axios.interceptors.request.use(
  (config) => {
   // do something before sending requests
   
    return config
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Intercept all responses
window.axios.interceptors.response.use(
(response) => {
 // when success from server
  
  return response;
},

(error) => {
 // when error from server
  Vue.$Progress.fail()
  return Promise.reject(error);
}
);
