<template>
  <div class="login">
    <form @submit.prevent="submitForm">
      <h2>Login</h2>
      <div class="input-group">
        <label for="username">Username</label>
        <input type="text" id="username" v-model="loginData.username" required />
      </div>
      <div class="input-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="loginData.password" required />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "LoginPage",
  data() {
    return {
      loginData: {
        username: "",
        password: "",
      },
    };
  },
  methods: {
    async submitForm() {
      try {
        const response = await axios.post(
          "http://localhost:5010/users/login",
          this.loginData
        );

        if (response.status === 200) {
          // Salvar o token de autenticação recebido em algum lugar
          // Isso poderia ser no local storage, por exemplo:
          window.localStorage.setItem("authToken", response.data.token);

          // Redirecionar o usuário para a página de tarefas
          this.$router.push("/tasks");
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
.login {
  width: 300px;
  margin: 0 auto;
  padding: 2em;
  box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.75);
  border-radius: 8px;
}
.login h2 {
  text-align: center;
  color: #2c3e50;
}
.input-group {
  margin-bottom: 1em;
}
.input-group label {
  display: block;
  margin-bottom: 0.5em;
}
.input-group input {
  width: 100%;
  padding: 0.5em;
  font-size: 0.9em;
  border-radius: 4px;
  border: 1px solid #ddd;
}
button {
  width: 100%;
  padding: 0.7em;
  background-color: #2c3e50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
button:hover {
  background-color: #1a242f;
}
</style>
