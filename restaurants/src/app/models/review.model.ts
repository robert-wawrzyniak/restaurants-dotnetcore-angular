export interface ReviewModel {
  restaurantId: string;
  userId: string;
  restaurant: string;
  user: string;
  visitDate: Date;
  rate: number;
  comment: string;
  reply?: string;
}
