import {NavItem} from './nav-item';

export const adminNavItems: NavItem[] = [
  {
    navCap: 'Home'
  },
  {
    displayName: 'Dashboards',
    iconName: 'home',
    route: 'dashboards',
    children: [
      {
        displayName: 'Analytical',
        iconName: 'point',
        route: 'dashboards/dashboard1',
      },
      {
        displayName: 'eCommerce',
        iconName: 'point',
        route: 'dashboards/dashboard2',
      },
    ],
  },
]
