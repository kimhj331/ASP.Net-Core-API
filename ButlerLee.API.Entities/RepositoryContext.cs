using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ButlerLee.API.Entities
{
    public partial class RepositoryContext : DbContext
    {
        public RepositoryContext()
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<UnpaidReservation> UnpaidReservation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=butlerlee.co9iacim4ksq.ap-northeast-2.rds.amazonaws.com;port=3306;userid=admin;password=4mARyv1xvmXJ9MHi5jG1;database=Butlerlee;guidformat=char36;treattinyasboolean=False", x => x.ServerVersion("10.4.21-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.HasComment("주문 결제 정보 테이블");

                entity.HasIndex(e => new { e.ReservationId, e.ReservationNo })
                    .HasName("payment_index1")
                    .IsUnique()
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned")
                    .HasComment("PK");

                entity.Property(e => e.ApproveDate)
                    .HasColumnName("approve_date")
                    .HasColumnType("datetime")
                    .HasComment("결제 승인 날짜");

                entity.Property(e => e.ArrivalDate)
                    .HasColumnName("arrival_date")
                    .HasColumnType("datetime")
                    .HasComment("숙박 시작일");

                entity.Property(e => e.CancelDate)
                    .HasColumnName("cancel_date")
                    .HasColumnType("datetime")
                    .HasComment("결제 취소 날짜");

                entity.Property(e => e.CanceledAmount)
                    .HasColumnName("canceled_amount")
                    .HasColumnType("int(11)")
                    .HasComment("취소된 금액");

                entity.Property(e => e.CardAcquireStatus)
                    .HasColumnName("card_ acquire_status")
                    .HasColumnType("varchar(20)")
                    .HasComment("카드 결제 매입 상태 (READY, REQUESTED, COMPLETED, CANCEL_REQUESTED, CANCELED)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardApproveNo)
                    .HasColumnName("card_approve_no")
                    .HasColumnType("varchar(25)")
                    .HasComment("카드사 승인 번호")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardCompany)
                    .HasColumnName("card_company")
                    .HasColumnType("varchar(20)")
                    .HasComment("카드사 이름")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardInstallmentPlanMonths)
                    .HasColumnName("card_installment_plan_months")
                    .HasColumnType("int(11)")
                    .HasComment("카드 할부 개월 수 (0: 일시불)");

                entity.Property(e => e.CardNumber)
                    .HasColumnName("card_number")
                    .HasColumnType("varchar(25)")
                    .HasComment("카드 번호")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardOwnerType)
                    .HasColumnName("card_owner_type")
                    .HasColumnType("varchar(20)")
                    .HasComment("카드 소유자 타입 (개인, 법인)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardReceiptUrl)
                    .HasColumnName("card_ receipt_url")
                    .HasColumnType("varchar(200)")
                    .HasComment("카드 매출전표 조회 페이지 주소")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CardType)
                    .HasColumnName("card_type")
                    .HasColumnType("varchar(20)")
                    .HasComment("카드 종류 (신용, 체크, 기프트)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.DepartureDate)
                    .HasColumnName("departure_date")
                    .HasColumnType("datetime")
                    .HasComment("숙박 마지막일");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnName("discount_amount")
                    .HasColumnType("int(11)")
                    .HasComment("카드사 즉시할인 프로모션 적용 금액");

                entity.Property(e => e.EasyPay)
                    .HasColumnName("easy_pay")
                    .HasColumnType("varchar(30)")
                    .HasComment("간편 결제 수단 (토스결제, 페이코, 삼성페이)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.IsInsterestFree)
                    .HasColumnName("is_insterest_free")
                    .HasComment("카드 무이자 할부 적용 여부");

                entity.Property(e => e.ListingId)
                    .HasColumnName("listing_id")
                    .HasColumnType("int(11)")
                    .HasComment("객실 ID");

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasColumnType("varchar(100)")
                    .HasComment("결제 수단 (카드, 가상계좌, 휴대폰, 계좌이체, 상품권(문화상품권, 도서문화상품권, 게임문화상품권))")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.OrderName)
                    .HasColumnName("order_name")
                    .HasColumnType("varchar(100)")
                    .HasComment("상품 이름")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PaymentGateway)
                    .IsRequired()
                    .HasColumnName("payment_gateway")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasComment("결제 대행사 (DIRECT_CREDIT_CARD, TOSS_PAY, KAKAO_PAY)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PaymentKey)
                    .HasColumnName("payment_key")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasComment("결제 키")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.RequestDate)
                    .HasColumnName("request_date")
                    .HasColumnType("datetime")
                    .HasComment("결제 요청 날짜");

                entity.Property(e => e.ReservationId)
                    .HasColumnName("reservation_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReservationNo)
                    .HasColumnName("reservation_no")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasComment("고객 제공 예약 ID")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.TaxFreeAmount)
                    .HasColumnName("tax_free_amount")
                    .HasColumnType("int(11)")
                    .HasComment("비과세 금액");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasColumnType("int(11)")
                    .HasComment("총 결제금액");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime")
                    .HasComment("결제 업데이트 날짜");
            });

            modelBuilder.Entity<UnpaidReservation>(entity =>
            {
                entity.ToTable("unpaid_reservation");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CancelDate)
                    .HasColumnName("cancel_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LimitStartDate)
                    .HasColumnName("limit_start_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.ReservationId)
                    .HasColumnName("reservation_id")
                    .HasColumnType("int(10)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
