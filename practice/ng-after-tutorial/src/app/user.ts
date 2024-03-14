export interface User {
  id: string;
  name: string;
  email: string;
  phone: string;
  company: {
    name: string;
  }
}

export const initialUser = {
  id: "",
  name: "",
  email: "",
  phone: "",
  company: {
    name: ""
  }
}
