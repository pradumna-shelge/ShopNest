import { createStore } from 'vuex';

const store = createStore({
  state: {
    cart: [], 
  },
  mutations: {
    addToCart(state, product) {
      state.cart.push(product);
      console.log(state.cart.length);
    },
     addList(state, product) {
      debugger;
      state.cart=product;
      console.log(state.cart.length);
    },
    removeFromCart(state, productId) {
      state.cart = state.cart.filter(item => item.id !== productId);
    },
    clearCart(state) {
      state.cart = [];
    },
  },
  actions: {
    async addItemToCart({ commit }, item) {
      try {
        await new Promise(resolve => setTimeout(resolve, 1000));
        commit('addToCart', item);
      } catch (error) {
        console.error('Error adding item to cart:', error);
      }
    },
    addList({ commit }, product) {

      commit('addList', product);
    },
    removeFromCart({ commit }, productId) {
      commit('removeFromCart', productId);
    },
    clearCart({ commit }) {
      commit('clearCart');
    },
  },
  getters: {
    cartItemCount: state => state.cart.length,
    cartItems: state => state.cart,
    cartTotalPrice:state=>parseFloat((state.cart.reduce((total, item) => total + (item.price * item.quantity), 0)).toFixed(2)),
    cartMrpPrice:state=>parseFloat((state.cart.reduce((total, item) => total + (item.mrprice * item.quantity), 0)).toFixed(2)),
    isProductflag: state => paramName => state.cart.some(item => item.productName === paramName),
  },
});

export default store;
