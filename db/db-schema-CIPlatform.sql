create database CIPlatform
use CIPlatform

Create table admin (
admin_id bigint IDENTITY(1,1) PRIMARY KEY ,
first_name varchar(16),
last_name varchar(16),
email varchar(128) not null,
password varchar(255) not null,
created_at datetime default current_timestamp,
updated_at datetime,
deleted_at datetime
)


Create table banner (
banner_id bigint IDENTITY(1,1) PRIMARY KEY ,
image varchar(512),
title text,
sort_order int,
created_at datetime default current_timestamp,
updated_at datetime,
deleted_at datetime
)


Create table country (
country_id bigint IDENTITY(1,1) PRIMARY KEY ,
name varchar(255) not null,
ISO varchar(16),
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

Create table city (
city_id bigint IDENTITY(1,1) PRIMARY KEY ,
country_id bigint foreign key references country(country_id),
created_at datetime default current_timestamp,
updated_at datetime,
deleted_at datetime
)

Create table cms_page (
cms_page_id bigint IDENTITY(1,1) PRIMARY KEY ,
title varchar(255),
description text,
slug varchar(255) not null,
status bit,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

Create table mission_theme (
mission_theme_id bigint IDENTITY(1,1) PRIMARY KEY ,
title varchar(255),
status tinyint not null default 1,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

Create table mission (
mission_id bigint IDENTITY(1,1) PRIMARY KEY ,
theme_id bigint foreign key references mission_theme(mission_theme_id) not null,
city_id bigint foreign key references city(city_id) not null,
country_id bigint foreign key references country(country_id) not null,
title varchar(128) not null,
short_description text,
description text,
start_date datetime default null,
end_date datetime default null,
mission_type varchar(20) NOT NULL CHECK (mission_type in ('TIME', 'GOAL')) ,
status bit,
organization_name varchar(255) default null,
organization_detail text default null,
availability varchar(20) CHECK (availability in ('daily', 'weekly', 'week-end', 'monthly')),
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null,
)

create table favourite_mission(
favourite_mission_id bigint IDENTITY(1,1) PRIMARY KEY ,
user_id bigint foreign key references [user](user_id),
mission_id bigint foreign key references mission(mission_id),
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)


create table goal_mission(
goal_mission_id bigint Identity(1,1) primary key,
mission_id bigint foreign key references mission(mission_id) not null,
goal_objective_text varchar(255),
goal_value int not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table [user](
user_id bigint Identity(1,1) primary key,
first_name varchar(16),
last_name varchar(16),
email varchar(128) not null,
password varchar(255) not null,
phone_number int not null,
avtar varchar(2048),
why_i_volunteer text,
employee_id varchar(16),
department varchar(16),
city_id bigint foreign key references city(city_id) not null,
country_id bigint foreign key references country(country_id) not null,
profile_text text,
linked_in_url varchar(255),
title varchar(255),
status bit,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)



create table mission_application(
mission_application_id bigint Identity(1,1) primary key,
mission_id bigint foreign key references mission(mission_id) not null,
user_id bigint foreign key references [user](user_id) not null,
applied_at datetime not null,
approval_status varchar(20) not null CHECK (approval_status in ('PENDING', 'DECLINE')) default 'PENDING',
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)


create table mission_document(
mission_document_id bigint Identity(1,1) primary key,
mission_id bigint foreign key references mission(mission_id) not null,
document_name varchar(255),
document_type varchar(255),
document_path varchar(255),
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table mission_invite(
mission_invite_id bigint Identity(1,1) primary key,
mission_id bigint foreign key references mission(mission_id) not null,
from_user_id bigint foreign key references [user](user_id) not null,
to_user_id bigint foreign key references [user](user_id) not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table mission_media(
mission_media_id bigint Identity(1,1) primary key,
mission_id bigint foreign key references mission(mission_id) not null,
media_name varchar(64),
media_type varchar(4),
media_path varchar(255),
defaultval bit default 0,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table mission_rating(
mission_rating_id bigint Identity(1,1) primary key,
user_id bigint foreign key references [user](user_id) not null,
mission_id bigint foreign key references mission(mission_id) not null,
rating tinyint not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table skill (
skill_id bigint identity(1,1) primary key,
skill_name varchar(64),
status tinyint not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)


create table password_reset(
email varchar(191) not null,
token varchar(191) not null,
created_at datetime default current_timestamp,
)


create table user_skill(
user_skill_id bigint identity(1,1) primary key,
user_id bigint foreign key references [user](user_id) not null,
skill_id bigint foreign key references skill(skill_id) not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)



create table timesheet(
timesheet_id bigint identity(1,1) primary key,
user_id bigint foreign key references [user](user_id) not null,
mission_id bigint foreign key references mission(mission_id) not null,
time time ,
action int,
date_volunteered datetime not null,
notes text ,
status varchar(20) not null CHECK (status in ('SUBMIT_FOR_APPROVAL', 'PENDING')) default 'PENDING',
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)


create table story(
story_id bigint identity(1,1) primary key,
user_id bigint foreign key references [user](user_id) not null,
mission_id bigint foreign key references mission(mission_id) not null,
title varchar(255),
description text,
status varchar(20) not null CHECK (status in ('DRAFT', 'DECLINED')) default 'DRAFT', 
published_at datetime default null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table story_invite(
story_invite_id bigint identity(1,1) primary key,
story_id bigint foreign key references story(story_id) not null,
from_user_id bigint foreign key references [user](user_id) not null,
to_user_id bigint foreign key references [user](user_id) not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table story_media(
story_media_id bigint identity(1,1) primary key,
story_id bigint foreign key references story(story_id) not null,
type varchar(8) not null,
path text not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table comment(
comment_id bigint identity(1,1) primary key,
user_id bigint foreign key references [user](user_id) not null,
mission_id bigint foreign key references mission(mission_id) not null,
approval_status varchar(20) not null CHECK (approval_status in ('PENDING', 'PUBLISHED')) default 'PENDING',
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)

create table mission_skill(
mission_skill_id bigint identity(1,1) primary key,
skill_id bigint foreign key references skill(skill_id) not null,
mission_id bigint foreign key references mission(mission_id) not null,
created_at datetime default current_timestamp,
updated_at datetime default null,
deleted_at datetime default null
)