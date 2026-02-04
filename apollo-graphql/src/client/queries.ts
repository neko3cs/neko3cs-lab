import { gql } from '@apollo/client';

export const GET_COFFEES = gql`
  query GetCoffees {
    coffees {
      id
      name
      price
      category
    }
  }
`;

export const GET_COFFEE_DETAIL = gql`
  query GetCoffeeDetail($id: ID!) {
    coffee(id: $id) {
      id
      name
      price
      category
      description
      options {
        size
        milk
        sugar
      }
    }
  }
`;
