interface String {
  format(placeholders: { [x: string]: string; }): string;
}

if (!String.prototype.format) {
  String.prototype.format = function() {
    var placeholders = arguments;
    var s = this;
    for(var propertyName in placeholders[0]) {
      s = s.replace(`{${propertyName}}`, placeholders[0][propertyName]);
    }    
    return s.toString();
  };
}