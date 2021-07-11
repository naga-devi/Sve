import { Menu } from './menu.model';

export const menuItems = [
    new Menu(10, 'Dashboard', '/admin', null, 'dashboard', null, false, 0),
    new Menu(20, 'Inventory', null, null, 'grid_on', null, true, 0),
    new Menu(22, 'Inventory List', '/admin/products/product-list', null, 'list', null, false, 20),
    new Menu(24, 'Add Inventory', '/admin/products/add-product', null, 'add_circle_outline', null, false, 20),
    new Menu(211, 'Settings', null, null, 'support', null, true, 20),
    new Menu(211, 'Group', '/admin/products/categories', null, 'add_circle_outline', null, false, 211),
    new Menu(212, 'Brands', '/admin/products/category-brands', null, 'add_circle_outline', null, false, 211),
    new Menu(213, 'Sizes', '/admin/products/category-sizes', null, 'add_circle_outline', null, false, 211),
    new Menu(214, 'Material Types', '/admin/products/category-material-types', null, 'add_circle_outline', null, false, 211),
    new Menu(215, 'Colors', '/admin/products/category-colors', null, 'add_circle_outline', null, false, 211),
    new Menu(216, 'Grades', '/admin/products/category-grades', null, 'add_circle_outline', null, false, 211),

    new Menu(30, 'Sales', null, null, 'monetization_on', null, true, 0),
    new Menu(31, 'Orders', '/admin/sales/orders', null, 'list_alt', null, false, 30),
    //new Menu(32, 'Reports', null, null, 'local_atm', null, true, 30),
    //new Menu(321, 'Day Ledger', null, null, 'multiline_chart', null, false, 32),
    new Menu(141, 'Reports', null, null, 'local_atm', null, true, 30),
    new Menu(142, 'Day Ledger', '/admin/sales/day-ledger', null, 'multiline_chart', null, false, 141),

    new Menu(300, 'Purchases', null, null, 'add_circle_outline', null, true, 0),
    new Menu(301, 'Orders', '/admin/purchases/orders', null, 'list_alt', null, false, 300),
    new Menu(302, 'Add Order', '/admin/purchases/add-purchase', null, 'add_circle_outline', null, false, 300),
    new Menu(303, 'Returns', '/admin/purchases/purchase-returns', null, 'list_alt', null, false, 300),
    new Menu(304, 'Credit Notes', '/admin/purchases/credit-notes', null, 'list_alt', null, false, 300),


    new Menu(400, 'Accounts', null, null, 'add_circle_outline', null, true, 0),
    new Menu(401, 'Payments', '/admin/accounts/transaction', null, 'list_alt', null, false, 400),
    new Menu(402, 'New Payment', '/admin/accounts/new-payment', null, 'add_circle_outline', null, false, 400),

    // new Menu(40, 'Users', '/admin/users', null, 'group_add', null, false, 0),
    new Menu(50, 'Vendors', '/admin/vendors', null, 'supervisor_account', null, false, 0),
    new Menu(501, 'Customers', '/admin/customers', null, 'supervisor_account', null, false, 0),
    // new Menu(60, 'Coupons', '/admin/coupons', null, 'card_giftcard', null, false, 0),
    // new Menu(70, 'Withdrawal', '/admin/withdrawal', null, 'credit_card', null, false, 0),
    new Menu(80, 'Analytics', '/admin/analytics', null, 'multiline_chart', null, false, 0),
    // new Menu(90, 'Refund', '/admin/refund', null, 'restore', null, false, 0),
    // new Menu(100, 'Followers', '/admin/followers', null, 'follow_the_signs', null, false, 0),
    // new Menu(110, 'Support', '/admin/support', null, 'support', null, false, 0),
    // new Menu(120, 'Reviews', '/admin/reviews', null, 'insert_comment', null, false, 0),
    // new Menu(200, 'External Link', null, 'http://themeseason.com', 'open_in_new', '_blank', false, 0)
]