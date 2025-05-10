import {SideBarItem} from './side-bar-item';
import {RouterConstants} from './router-constants';

export const AdminSideBarData: SideBarItem[] = [
  {
    icon: 'home',
    path: RouterConstants.ADMIN_HOME,
    name: 'Home'
  },
  {
    icon: 'person',
    path: RouterConstants.ADMIN_USERS,
    name: 'Users'
  }
]

export const AuditorSideBarData: SideBarItem[] = [
  {
    icon: 'home',
    path: RouterConstants.AUDIT_HOME,
    name: 'Home'
  }
]
