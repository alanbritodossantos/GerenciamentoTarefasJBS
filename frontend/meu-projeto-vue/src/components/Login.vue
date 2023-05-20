<template>
  <div>
    <h2>Login</h2>
    <form @submit.prevent="submitForm">
      <div>
        <label for="username">Username:</label>
        <input id="username" v-model="loginData.username" type="text" required>
      </div>
      <div>
        <label for="password">Password:</label>
        <input id="password" v-model="loginData.password" type="password" required>
      </div>
      <div>
        <button type="submit">Login</button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'LoginPage',
  data() {
    return {
      loginData: {
        username: '',
        password: ''
      }
    }
  },
  methods: {
    async submitForm() {
      try {
        const response = await axios.post('http://localhost:5010/users/login', this.loginData);

        if (response.status === 200) {
          // Salvar o token de autenticação recebido em algum lugar
          // Isso poderia ser no local storage, por exemplo:
          window.localStorage.setItem('authToken', response.data.token);

          // Redirecionar o usuário para a página de tarefas
          this.$router.push('/tasks');
        } else {
          // Tratar erros retornados pelo servidor
          console.error(response.data.message);
        }
      } catch (error) {
        // Tratar erros que ocorrerem ao fazer a requisição
        console.error(error);
      }
    }
  }
}
</script>

<style scoped>
/* estilo */
</style>
