<template>
  <div>
    <header class="bg-white shadow">
      <div class="container mx-auto px-4 py-6 flex justify-between items-center">
        <!-- Logo and Navigation Links -->
        <NuxtLink to="/" class="text-xl font-bold">Task Manager</NuxtLink>
        <nav>
          <ul class="flex space-x-6">
            <li>
              <NuxtLink 
                to="/" 
                class="hover:text-blue-500 transition-colors"
                :class="{'text-blue-500 font-medium': $route.path === '/'}"
              >
              <!-- Home Link -->
                Tasks
              </NuxtLink>
            </li>
            <li>
              <NuxtLink 
                to="/tags" 
                class="hover:text-blue-500 transition-colors"
                :class="{'text-blue-500 font-medium': $route.path === '/tags'}"
              >
              <!-- Tags Link -->
                Tags
              </NuxtLink>
            </li>
          </ul>
        </nav>
      </div>
    </header>
    
    <!-- Main Content Area -->
    <main class="container mx-auto px-4 py-8">
      <NuxtPage />
    </main>
  </div>
</template>

<!-- This is the main layout for the Nuxt.js application. It includes a header with navigation links and a main content area where pages will be rendered. -->
<style>
.router-link-exact-active {
  color: #3b82f6; /* Tailwind's blue-500 */
  font-weight: 500;
}
</style>