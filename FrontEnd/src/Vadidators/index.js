

export  function  CustomValidationMsg(msg,key){
let cusMassage = ""
debugger;
    switch(msg){
        case  "Value is required":
            cusMassage="*"+key+" "+" is required";
            break;

            case "*Value is not a valid email address":
               cusMassage="*Please enter valid email address "; 
               break;
            case "The value must be equal to the other value":
                cusMassage="*Password does not match";
                break;

            default:
                cusMassage="*At least 8 characters (1 uppercase, 1 lowercase, 1 number, 1 special character)";
                break;
            
    }

return cusMassage
}


export const passwordValidator = (value) => {
  if (!value) {
    return true; 
  }


  const isLengthValid = value.length >= 8 && value.length <= 20;

 
  const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/;
  const isStrongPassword = strongPasswordRegex.test(value);

  return isLengthValid && isStrongPassword;

};
