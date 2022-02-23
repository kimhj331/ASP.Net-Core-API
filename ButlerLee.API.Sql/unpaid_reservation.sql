CREATE TABLE `unpaid_reservation` (
  `id` 			      int AUTO_INCREMENT NOT NULL,
  `reservation_id`    int                NOT NULL, -- hostaway reservation id
  `limit_start_date`  datetime NOT NULL, -- 예약일시
  `cancel_date`       datetime , -- 생성일
  PRIMARY KEY (id)
) ENGINE=InnoDB;
