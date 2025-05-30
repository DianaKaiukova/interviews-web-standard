<template>
  <div>
    <header class="bg-white shadow">
      <div class="container mx-auto px-4 py-6 flex justify-between items-center">
        <NuxtLink to="/" class="text-xl font-bold">Task Manager</NuxtLink>
        <nav>
          <ul class="flex space-x-6">
            <li>
              <NuxtLink 
                to="/" 
                class="hover:text-blue-500 transition-colors"
                :class="{'text-blue-500 font-medium': $route.path === '/'}"
              >
                Tasks
              </NuxtLink>
            </li>
            <li>
              <NuxtLink 
                to="/tags" 
                class="hover:text-blue-500 transition-colors"
                :class="{'text-blue-500 font-medium': $route.path === '/tags'}"
              >
                Tags
              </NuxtLink>
            </li>
          </ul>
        </nav>
      </div>
    </header>
    
    <main class="container mx-auto px-4 py-8">
      <NuxtPage />
    </main>
  </div>
</template>

<style>
.router-link-exact-active {
  color: #3b82f6; /* Tailwind's blue-500 */
  font-weight: 500;
}
</style>