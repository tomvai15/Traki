import axios from 'axios';

// TODO: fix to use env
//const API_URL = process.env.BASE_URL;
const API_URL = 'http://10.0.2.2:3004/';

class ApiService {
  static async get<Reqeust>(path: string): Promise<Reqeust> {
    const url = API_URL + path;
    const response = await axios.get(url, { headers: {} });
    return response.data;
  } 
}

export default ApiService;