export function formatDate(date) {
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const hours = date.getHours().toString().padStart(2, '0');
  const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');

return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;}

export function addDecimalIfMissing(input) {                              
    
  const inputString = typeof input === 'string' ? input : input.toString();
 
  const formattedString = inputString.replace(/^(.*\..*|\d*)$/, '$1.00');
  
  return formattedString;
}              