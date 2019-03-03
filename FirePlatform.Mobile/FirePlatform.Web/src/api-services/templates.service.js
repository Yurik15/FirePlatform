import Axios from 'axios';
 
const RESOURCE_NAME = '/api';
const env = 'https://localhost:44358';
 
export default {
  getAll() {
    return Axios.get(env + RESOURCE_NAME + '/Calculations/LoadTemplates');
  },
  loadTemplatesData(numberTmpl)
  {
    return Axios.get(env + RESOURCE_NAME + '/Calculations/LoadTmp?numberTmpl=' + numberTmpl, process.env.API_ENDPOINT);
  },
  get(id) {
    return Axios.get($,{RESOURCE_NAME}/$,{id});
     }, 
    create(data) { return Axios.post(RESOURCE_NAME, data); }, 
    update(id, data) { return Axios.put($,{RESOURCE_NAME}/$,{id}, data); }, 
    delete(id) { return Axios.delete($,{RESOURCE_NAME}/$,{id}); }
}