


export function storeScrollPosition() {
 
  scrollPosition.value = window.scrollY;
}

export function restoreScrollPosition() {
 
  return scrollPosition.value
}

export function resetScroll(){
  scrollPosition.value = 0
}
import { ref } from "vue";
let scrollPosition = ref( 0);