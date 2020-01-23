create user 'root'@'%' identified by 'cd40';
grant all privileges on *.* to 'root'@'%' with grant option;
grant all privileges on *.* to 'root'@'localhost' with grant option;