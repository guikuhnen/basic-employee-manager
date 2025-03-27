import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authenticated: boolean =
    localStorage.getItem('authenticated') == 'true';

  if (authenticated && state.url == '/login') {
    router.navigate(['/employee-list']);
    return false;
  } else if (authenticated) {
    if (state.url.indexOf('/employee-edit/') >= 0) {
      if (Number.isInteger(parseInt(state.url.substr(-1)))) {
        return true;
      } else {
        router.navigate(['/employee-list']);
        return false;
      }
    } else {
      return true;
    }
  } else if (!authenticated && state.url == '/login') {
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};
