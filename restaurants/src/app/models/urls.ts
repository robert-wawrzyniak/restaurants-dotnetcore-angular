export enum Urls {
  register = 'register',
  userMain = 'user/main',
  ownerMain = 'owner/main',
  adminMain = 'admin/main',
  adminEditUser = 'admin/user/:id',
  restaurantDetails = 'restaurant/details/:id',
  restaurantAdd = 'restaurant/add',
  restaurantEdit = 'restaurant/edit/:id',
  review = 'restaurant/:id/review/add',
  reviewReply = 'restaurant/:restaurantId/review/:userId/reply'
}

export function urlWithParams(url: string, params: any): string {
  Object.keys(params).forEach(key => {
    url = url.replace(':' + key, params[key]);
  });

  return `/${url}`;
}
