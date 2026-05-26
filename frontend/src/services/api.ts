import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'http://localhost:5132/api', // Passe die URL entsprechend an
  headers: {
    'Content-Type': 'application/json'
  }
})

export default apiClient
