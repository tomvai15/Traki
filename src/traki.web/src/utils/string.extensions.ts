/* eslint-disable */ 

declare global {  
  interface String {
    format(placeholders: { [x: string]: string; }): string;
  }
}  

String.prototype.format = function() {
  const placeholders = arguments;
  let s = this;
  for(const propertyName in placeholders[0]) {
    s = s.replace(`{${propertyName}}`, placeholders[0][propertyName]);
  }    
  return s.toString();
}; 
export {};