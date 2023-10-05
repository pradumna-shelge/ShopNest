<script setup lang='js'>
import logo from '../assets/logo.png'

</script>
<template>
 <section class="bg-gray-900">
        <div class="container mx-auto py-4 lg:py-6">
            <div class="md:flex md:justify-between">
                <div class="mb-6 md:mb-0">
                      <router-link to="/" >
      <a   class="flex items-center">
          <img  :src="logo" class=" logo" alt="ShopNest Logo">
          <span class="self-center text-2xl font-semibold whitespace-nowrap text-white"><router-link to="/" >ShopNest</router-link></span>
        </a>  </router-link>
                </div>
                <div class="grid grid-cols-2 gap-4 md:gap-6 sm:grid-cols-3">
                    <div>
                        <h2 class="mb-4 text-sm font-semibold text-gray-900 uppercase">Resources</h2>
                        <ul class="text-gray-500 font-medium">
                            <li class="mb-2">
                                <a href="#" class="hover:underline">ShopNest</a>
                            </li>
                            
                        </ul>
                    </div>
                    <!-- <div>
                        <h2 class="mb-4 text-sm font-semibold text-gray-900 uppercase">Follow us</h2>
                        <ul class="text-gray-500 font-medium">
                            <li class="mb-2">
                                <a href="#" class="hover:underline">Github</a>
                            </li>
                            <li>
                                <a href="#" class="hover:underline">Discord</a>
                            </li>
                        </ul>
                    </div> -->
                    <div>
                        <h2 class="mb-4 text-sm font-semibold text-gray-900 uppercase">Legal</h2>
                        <ul class="text-gray-500 font-medium">
                            <li class="mb-2">
                                <a href="#" class="hover:underline">Privacy Policy</a>
                            </li>
                            <li>
                                <a href="#" class="hover:underline">Terms & Conditions</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <hr class="my-6 border-gray-200 sm:mx-auto border-gray-700 lg:my-8" />
            <div class="sm:flex sm:items-center sm:justify-between">
                <span class="text-sm text-gray-500 sm:text-center text-gray-400">© 2023 <a href="#"
                        class="hover:underline">ShopNest™</a>. All Rights Reserved.</span>
                <div class="flex mt-4 space-x-4 sm:justify-center sm:mt-0">
                    <a href="#" class="text-gray-500 hover:text-gray-900 hover:text-white">
                        <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 8 19">
                            <path fill-rule="evenodd"
                                d="M6.135 3H8V0H6.135a4.147 4.147 0 0 0-4.142 4.142V6H0v3h2v9.938h3V9h2.021l.592-3H5V3.591A.6.6 0 0 1 5.592 3h.543Z"
                                clip-rule="evenodd" />
                        </svg>
                        <span class="sr-only">Facebook page</span>
                    </a>
                    <a href="#" class="text-gray-500 hover:text-gray-900 hover:text-white">
                        <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 21 16">
                            <path
                                d="M16.942 1.556a16.3 16.3 0 0 0-4.126-1.3 12.04 12.04 0 0 0-.529 1.1 15.175 15.175 0 0 0-4.573 0 11.585 11.585 0 0 0-.535-1.1 16.274 16.274 0 0 0-4.129 1.3A17.392 17.392 0 0 0 .182 13.218a15.785 15.785 0 0 0 4.963 2.521c.41-.564.773-1.16 1.084-1.785a10.63 10.63 0 0 1-1.706-.83c.143-.106.283-.217.418-.33a11.664 11.664 0 0 0 10.118 0c.137.113.277.224.418.33-.544.328-1.116.606-1.71.832a12.52 12.52 0 0 0 1.084 1.785 16.46 16.46 0 0 0 5.064-2.595 17.286 17.286 0 0 0-2.973-11.59ZM6.678 10.813a1.941 1.941 0 0 1-1.8-2.045 1.93 1.93 0 0 1 1.8-2.047 1.919 1.919 0 0 1 1.8 2.047 1.93 1.93 0 0 1-1.8 2.045Zm6.644 0a1.94 1.94 0 0 1-1.8-2.045 1.93 1.93 0 0 1 1.8-2.047 1.918 1.918 0 0 1 1.8 2.047 1.93 1.93 0 0 1-1.8 2.045Z" />
                        </svg>
                        <span class="sr-only">Discord community</span>
                    </a>
                    <a href="#" class="text-gray-500 hover:text-gray-900 hover:text-white">
                        <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 20 17">
                            <path fill-rule="evenodd"
                                d="M20 1.892a8.178 8.178 0 0 1-2.355.635 4.074 4.074 0 0 0 1.8-2.235 8.344 8.344 0 0 1-2.605.98A4.13 4.13 0 0 0 13.85 0a4.068 4.068 0 0 0-4.1 4.038 4 4 0 0 0 .105.919A11.705 11.705 0 0 1 1.4.734a4.006 4.006 0 0 0 1.268 5.392 4.165 4.165 0 0 1-1.859-.5v.05A4.057 4.057 0 0 0 4.1 9.635a4.19 4.19 0 0 1-1.856.07 4.108 4.108 0 0 0 3.831 2.807A8.36 8.36 0 0 1 0 14.184 11.732 11.732 0 0 0 6.291 16 11.502 11.502 0 0 0 17.964 4.5c0-.177 0-.35-.012-.523A8.143 8.143 0 0 0 20 1.892ZM6.678 10.813a1.941 1.941 0 0 1-1.8-2.045 1.93 1.93 0 0 1 1.8-2.047 1.919 1.919 0 0 1 1.8 2.047 1.93 1.93 0 0 1-1.8 2.045Zm6.644 0a1.94 1.94 0 0 1-1.8-2.045 1.93 1.93 0 0 1 1.8-2.047 1.918 1.918 0 0 1 1.8 2.047 1.93 1.93 0 0 1-1.8 2.045Z" />
                        </svg>
                        <span class="sr-only">Twitter page</span>
                    </a>
                    <a href="#" class="text-gray-500 hover:text-gray-900 hover:text-white">
                        <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 20 20">
                            <path fill-rule="evenodd"
                                d="M10 .333A9.911 9.911 0 1 0 10 19.667 9.911 9.911 0 0 0 10 .333ZM16.613 7.377a6.238 6.238 0 0 0 1.933-2.306 6.363 6.363 0 0 1-1.774.486 3.143 3.143 0 0 0 1.371-1.735 6.274 6.274 0 0 1-1.996.761 3.122 3.122 0 0 0-2.326-1.055c-1.71 0-3.097 1.364-3.097 3.048 0 .24.027.474.08.704a8.908 8.908 0 0 0-6.446-3.24c-1.71 0-3.098 1.364-3.098 3.047 0 .24.028.475.08.705A9.017 9.017 0 0 0 .852 8.22a3.13 3.13 0 0 0-.37 1.496c0 1.04.472 1.95 1.204 2.495a3.134 3.134 0 0 1-1.405-.39c-.036 1.143.832 2.212 2.02 2.442a6.337 6.337 0 0 1-1.635.22c-.394 0-.78-.042-1.155-.12a8.902 8.902 0 0 0 4.817 1.418c5.862 0 9.055-4.845 9.055-9.049 0-.137-.003-.275-.008-.41a6.424 6.424 0 0 0 1.582-1.65c-.585.27-1.217.44-1.878.517Z"
                                clip-rule="evenodd" />
                        </svg>
                        <span class="sr-only">GitHub account</span>
                    </a>
                    <a href="#" class="text-gray-500 hover:text-gray-900 hover:text-white">
                        <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor"
                            viewBox="0 0 20 20">
                            <path fill-rule="evenodd"
                                d="M10 .333A10 10 0 0 0 3.115 15.673a11.862 11.862 0 0 1 1.747-.134 8.392 8.392 0 0 0-.426 2.62c0 1.84.78 3.477 2.018 4.635-.74.216-1.52.33-2.319.33a4.897 4.897 0 0 1-1.665-.282c.595 1.746 2.313 3.02 4.354 3.057a9.885 9.885 0 0 1-6.04 2.076A10.07 10.07 0 0 0 10 19.666c6.1 0 9.437-5.172 9.437-9.637 0-.146 0-.292-.01-.438a6.864 6.864 0 0 0 1.697-1.745 6.778 6.778 0 0 0-1.85-.25 6.778 6.778 0 0 0-1.85.25 6.866 6.866 0 0 0 1.546 1.735c-.06.146-.12.29-.187.433a9.927 9.927 0 0 0 2.524-2.05c.57-.598 1.04-1.276 1.404-2.01a6.74 6.74 0 0 0-1.602-.21c-.23 0-.465.02-.698.063a6.866 6.866 0 0 0-5.76 3.01ZM2.122 14.036a8.498 8.498 0 0 1-.242-2.046c0-.71.08-1.41.23-2.104a7.63 7.63 0 0 0-2.06 2.624 8.658 8.658 0 0 1-.252 1.815 10.02 10.02 0 0 1-.04-1.982c0-.68.06-1.362.18-2.04a11.14 11.14 0 0 0 1.014-4.68c0-.625.08-1.25.236-1.87a11.006 11.006 0 0 1 5.387 11.45c-.45-.2-.9-.394-1.35-.58Z"
                                clip-rule="evenodd" />
                        </svg>
                        <span class="sr-only">Dribbble account</span>
                    </a>
                </div>
            </div>
        </div>
    </section>

</template>

<style scoped>
</style>