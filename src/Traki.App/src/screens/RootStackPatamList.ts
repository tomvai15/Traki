export type RootStackParamList = {
  Home: undefined;
  Details: { id: number, info: string };
  Feed: { sort: 'latest' | 'top' } | undefined;
};