<template>
  <div class="register">
    <form @submit.prevent="submitForm">
      <h2>Register</h2>
      <div class="input-group">
        <label for="username">Username</label>
        <input type="text" id="username" v-model="registerData.username" required />
      </div>
      <div class="input-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="registerData.password" required />
      </div>
      <button type="submit">Register</button>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "RegisterPage",
  data() {
    return {
      registerData: {
        username: "",
        password: "",
      },
    };
  },
  methods: {
    async submitForm() {
      try {
        const response = await axios.post(
          "http://localhost:5010/users/register",
          this.registerData
        );

        if (response.status === 200) {
          // Redirecionar o usuário para a página de login
          this.$router.push("/login");
        } else {
          // Tratar erros retornados pelo servidor
          console.error(response.data.message);
        }
      } catch (error) {
        // Tratar erros que ocorrerem ao fazer a requisição
        console.error(error);
      }
    },
  },
};
</script>

<style scoped>
/* A estilização será similar à do login */
</style>
