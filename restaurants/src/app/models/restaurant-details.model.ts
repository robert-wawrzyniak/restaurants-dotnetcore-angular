import { RestaurantModel } from './restaurant.model';
import { UserModel } from './user.model';
import { ReviewModel } from './review.model';

export interface RestaurantDetailsModel extends RestaurantModel {
  owner: UserModel;
  averageRating: number;
  highestRatedReview: ReviewModel;
  lowestRatedReview: ReviewModel;
  lastReviews: ReviewModel[];
}
