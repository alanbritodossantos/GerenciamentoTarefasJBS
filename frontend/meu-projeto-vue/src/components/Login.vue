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
    submitForm() {
      // função
    }
  }
}
</script>

<style scoped>
/* estilo */
</style>
