using System.Data.Entity;

namespace iManage.Api
{
    internal class iMDbContext : BaseDbContext
    {
        public iMDbContext()
            : base("active_conStr")
        {
            Database.SetInitializer<iMDbContext>(new iMDbInit());
        }

        public virtual DbSet<APP> APPS { get; set; }
        public virtual DbSet<ARCHIVETBL> ARCHIVETBLs { get; set; }
        public virtual DbSet<ARSET_INFO> ARSET_INFO { get; set; }
        public virtual DbSet<BIG_SID_TABLE> BIG_SID_TABLE { get; set; }
        public virtual DbSet<CHANGE_EVENT_PROPERTIES> CHANGE_EVENT_PROPERTIES { get; set; }
        public virtual DbSet<CHANGE_EVENT_QUEUES> CHANGE_EVENT_QUEUES { get; set; }
        public virtual DbSet<CHANGE_EVENTS> CHANGE_EVENTS { get; set; }
        public virtual DbSet<CHECKOUT> CHECKOUTs { get; set; }
        public virtual DbSet<CUSTOM1> CUSTOM1 { get; set; }
        public virtual DbSet<CUSTOM10> CUSTOM10 { get; set; }
        public virtual DbSet<CUSTOM11> CUSTOM11 { get; set; }
        public virtual DbSet<CUSTOM12> CUSTOM12 { get; set; }
        public virtual DbSet<CUSTOM2> CUSTOM2 { get; set; }
        public virtual DbSet<CUSTOM29> CUSTOM29 { get; set; }
        public virtual DbSet<CUSTOM3> CUSTOM3 { get; set; }
        public virtual DbSet<CUSTOM30> CUSTOM30 { get; set; }
        public virtual DbSet<CUSTOM31> CUSTOM31 { get; set; }
        public virtual DbSet<CUSTOM4> CUSTOM4 { get; set; }
        public virtual DbSet<CUSTOM5> CUSTOM5 { get; set; }
        public virtual DbSet<CUSTOM6> CUSTOM6 { get; set; }
        public virtual DbSet<CUSTOM7> CUSTOM7 { get; set; }
        public virtual DbSet<CUSTOM8> CUSTOM8 { get; set; }
        public virtual DbSet<CUSTOM9> CUSTOM9 { get; set; }
        public virtual DbSet<DOC_ACCESS> DOC_ACCESS { get; set; }
        public virtual DbSet<DOC_DENIAL> DOC_DENIAL { get; set; }
        public virtual DbSet<DOC_INDEX> DOC_INDEX { get; set; }
        public virtual DbSet<DOC_KEYWORDS> DOC_KEYWORDS { get; set; }
        public virtual DbSet<DOC_NVPS> DOC_NVPS { get; set; }
        public virtual DbSet<DOCARTIFACT> DOCARTIFACTs { get; set; }
        public virtual DbSet<DOCCACHE> DOCCACHEs { get; set; }
        public virtual DbSet<DOCCLASS> DOCCLASSES { get; set; }
        public virtual DbSet<DOCMASTER> DOCMASTERs { get; set; }
        public virtual DbSet<DOCSERVER> DOCSERVERS { get; set; }
        public virtual DbSet<DOCSUBCLASS> DOCSUBCLASSES { get; set; }
        public virtual DbSet<DOCTYPE> DOCTYPES { get; set; }
        public virtual DbSet<DOCUSER> DOCUSERS { get; set; }
        public virtual DbSet<DSECURITY> DSECURITies { get; set; }
        public virtual DbSet<EM_PROJECTS> EM_PROJECTS { get; set; }
        public virtual DbSet<EM_REQUESTS> EM_REQUESTS { get; set; }
        public virtual DbSet<EMAIL_ATTRS> EMAIL_ATTRS { get; set; }
        public virtual DbSet<FORM_CONTROLS> FORM_CONTROLS { get; set; }
        public virtual DbSet<FORM_TEMPLATE> FORM_TEMPLATE { get; set; }
        public virtual DbSet<FORM_TYPES> FORM_TYPES { get; set; }
        public virtual DbSet<FORM> FORMS { get; set; }
        public virtual DbSet<GROUP> GROUPS { get; set; }
        public virtual DbSet<GRP_SCTY_ACC_TEMP> GRP_SCTY_ACC_TEMP { get; set; }
        public virtual DbSet<INDEX_COLLECTION> INDEX_COLLECTION { get; set; }
        public virtual DbSet<KEYWORD> KEYWORDS { get; set; }
        public virtual DbSet<KMTAG> KMTAGs { get; set; }
        public virtual DbSet<LIBRARy> LIBRARIES { get; set; }
        public virtual DbSet<NODELOC> NODELOCs { get; set; }
        public virtual DbSet<PALETTE> PALETTEs { get; set; }
        public virtual DbSet<PROJ_ACCESS> PROJ_ACCESS { get; set; }
        public virtual DbSet<PROJ_DENIAL> PROJ_DENIAL { get; set; }
        public virtual DbSet<PROJARTIFACT> PROJARTIFACTs { get; set; }
        public virtual DbSet<PROJECT_ITEMS> PROJECT_ITEMS { get; set; }
        public virtual DbSet<PROJECT_NVPS> PROJECT_NVPS { get; set; }
        public virtual DbSet<PROJECT> PROJECTS { get; set; }
        public virtual DbSet<REEVENTDEF> REEVENTDEFs { get; set; }
        public virtual DbSet<REEVENTLOG> REEVENTLOGs { get; set; }
        public virtual DbSet<RELATED_DOCS> RELATED_DOCS { get; set; }
        public virtual DbSet<RERULEHANDLER> RERULEHANDLERS { get; set; }
        public virtual DbSet<RERULE> RERULES { get; set; }
        public virtual DbSet<ROLE_NVPS> ROLE_NVPS { get; set; }
        public virtual DbSet<ROLE_PROFILES> ROLE_PROFILES { get; set; }
        public virtual DbSet<ROLE> ROLES { get; set; }
        public virtual DbSet<SEARCH_PROFILES> SEARCH_PROFILES { get; set; }
        public virtual DbSet<SECURITY_TEMPLATE> SECURITY_TEMPLATE { get; set; }
        public virtual DbSet<SID_TABLE> SID_TABLE { get; set; }
        public virtual DbSet<SRCH_PROF_ACCESS> SRCH_PROF_ACCESS { get; set; }
        public virtual DbSet<SRCH_PROF_DENIAL> SRCH_PROF_DENIAL { get; set; }
        public virtual DbSet<SSR_RETRY> SSR_RETRY { get; set; }
        public virtual DbSet<SYNC_ITEMS> SYNC_ITEMS { get; set; }
        public virtual DbSet<SYNC_LOCATIONS> SYNC_LOCATIONS { get; set; }
        public virtual DbSet<SYNC_PREFERENCES> SYNC_PREFERENCES { get; set; }
        public virtual DbSet<SYSTEM_PREFERENCES> SYSTEM_PREFERENCES { get; set; }
        public virtual DbSet<SYSTEM_SYNC_PREFS> SYSTEM_SYNC_PREFS { get; set; }
        public virtual DbSet<TEMPLATE_CONTROLS> TEMPLATE_CONTROLS { get; set; }
        public virtual DbSet<USER_PREFERENCES> USER_PREFERENCES { get; set; }
        public virtual DbSet<USER_SYNC_PREFS> USER_SYNC_PREFS { get; set; }
        public virtual DbSet<USERACTION> USERACTIONS { get; set; }
        public virtual DbSet<USERHISTORY> USERHISTORies { get; set; }
        public virtual DbSet<USR_SCTY_ACC_TEMP> USR_SCTY_ACC_TEMP { get; set; }
        public virtual DbSet<USR_SCTY_TEMP_ASSC> USR_SCTY_TEMP_ASSC { get; set; }
        public virtual DbSet<WEB_PAGE_ACCESS> WEB_PAGE_ACCESS { get; set; }
        public virtual DbSet<CACHE_UPDATE> CACHE_UPDATE { get; set; }
        public virtual DbSet<CAPTION> CAPTIONS { get; set; }
        public virtual DbSet<MOVED_PROJECTS> MOVED_PROJECTS { get; set; }
        public virtual DbSet<QUICK_RETRIEVE> QUICK_RETRIEVE { get; set; }
        public virtual DbSet<RESTORETBL> RESTORETBLs { get; set; }
        public virtual DbSet<SYSTEM_MANAGEMENT> SYSTEM_MANAGEMENT { get; set; }
        public virtual DbSet<TYPEMAP> TYPEMAPs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APP>()
                .Property(e => e.PRIMARYAPP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<APP>()
                .Property(e => e.DDE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<APP>()
                .Property(e => e.ODMA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ARCHIVETBL>()
                .HasMany(e => e.ARSET_INFO)
                .WithRequired(e => e.ARCHIVETBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARSET_INFO>()
                .Property(e => e.RESTORED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BIG_SID_TABLE>()
                .Property(e => e.TABLE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CHANGE_EVENT_PROPERTIES>()
                .Property(e => e.VALUE_DATE)
                .IsFixedLength();

            modelBuilder.Entity<CHANGE_EVENTS>()
                .HasMany(e => e.CHANGE_EVENT_PROPERTIES)
                .WithRequired(e => e.CHANGE_EVENTS)
                .HasForeignKey(e => e.EVENT_RSID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHECKOUT>()
                .Property(e => e.PORTABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM1>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM1>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM1>()
                .HasMany(e => e.CUSTOM2)
                .WithRequired(e => e.CUSTOM1)
                .HasForeignKey(e => e.CPARENT_ALIAS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CUSTOM1>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM1)
                .HasForeignKey(e => e.C1ALIAS);

            modelBuilder.Entity<CUSTOM10>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM10>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM10>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM10)
                .HasForeignKey(e => e.C10ALIAS);

            modelBuilder.Entity<CUSTOM11>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM11>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM11>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM11)
                .HasForeignKey(e => e.C11ALIAS);

            modelBuilder.Entity<CUSTOM12>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM12>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM12>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM12)
                .HasForeignKey(e => e.C12ALIAS);

            modelBuilder.Entity<CUSTOM2>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM2>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM2>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM2)
                .HasForeignKey(e => new { e.C1ALIAS, e.C2ALIAS });

            modelBuilder.Entity<CUSTOM29>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM29>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM29>()
                .HasMany(e => e.CUSTOM30)
                .WithRequired(e => e.CUSTOM29)
                .HasForeignKey(e => e.CPARENT_ALIAS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CUSTOM29>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM29)
                .HasForeignKey(e => e.C29ALIAS);

            modelBuilder.Entity<CUSTOM3>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM3>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM3>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM3)
                .HasForeignKey(e => e.C3ALIAS);

            modelBuilder.Entity<CUSTOM30>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM30>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM30>()
                .HasMany(e => e.CUSTOM31)
                .WithRequired(e => e.CUSTOM30)
                .HasForeignKey(e => new { e.CPARENT_ALIAS, e.CGRANDPARENT_ALIAS })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CUSTOM30>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM30)
                .HasForeignKey(e => new { e.C30ALIAS, e.C29ALIAS });

            modelBuilder.Entity<CUSTOM31>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM31>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM31>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM31)
                .HasForeignKey(e => new { e.C31ALIAS, e.C30ALIAS, e.C29ALIAS });

            modelBuilder.Entity<CUSTOM4>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM4>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM4>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM4)
                .HasForeignKey(e => e.C4ALIAS);

            modelBuilder.Entity<CUSTOM5>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM5>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM5>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM5)
                .HasForeignKey(e => e.C5ALIAS);

            modelBuilder.Entity<CUSTOM6>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM6>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM6>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM6)
                .HasForeignKey(e => e.C6ALIAS);

            modelBuilder.Entity<CUSTOM7>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM7>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM7>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM7)
                .HasForeignKey(e => e.C7ALIAS);

            modelBuilder.Entity<CUSTOM8>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM8>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM8>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM8)
                .HasForeignKey(e => e.C8ALIAS);

            modelBuilder.Entity<CUSTOM9>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM9>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CUSTOM9>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.CUSTOM9)
                .HasForeignKey(e => e.C9ALIAS);

            modelBuilder.Entity<DOC_ACCESS>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOC_DENIAL>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOC_INDEX>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCACHE>()
                .Property(e => e.LATESTVERSION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCACHE>()
                .Property(e => e.DELETED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCACHE>()
                .Property(e => e.DOCINUSE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCACHE>()
                .Property(e => e.UPLOAD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCLASS>()
                .Property(e => e.INDEXABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCLASS>()
                .Property(e => e.SHADOW)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCLASS>()
                .Property(e => e.SUBCLASS_REQD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCLASS>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCCLASS>()
                .HasMany(e => e.DOCSUBCLASSES)
                .WithRequired(e => e.DOCCLASS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.DOCINUSE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.INDEXED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.CHECKEDOUT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.COMINDEX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.ARCHIVE_REQ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.ARCHIVED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.INDEXABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.ISRELATED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.CBOOL1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.CBOOL2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.CBOOL3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.CBOOL4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.PENDING_CLOSE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.DEFAULT_SECURITY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.IS_SECURED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.IS_EXTERNAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.HAS_ATTACHMENT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.EXTRNL_AS_NRML)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCMASTER>()
                .HasMany(e => e.KMTAGs)
                .WithOptional(e => e.DOCMASTER)
                .HasForeignKey(e => new { e.DOCNUM, e.VERSION });

            modelBuilder.Entity<DOCMASTER>()
                .HasMany(e => e.PROJECTS)
                .WithOptional(e => e.DOCMASTER)
                .HasForeignKey(e => new { e.DOCNUM, e.VERSION });

            modelBuilder.Entity<DOCSERVER>()
                .Property(e => e.READONLY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSERVER>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSERVER>()
                .Property(e => e.TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSERVER>()
                .Property(e => e.SECURE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSERVER>()
                .HasMany(e => e.DOCUSERS)
                .WithOptional(e => e.DOCSERVER1)
                .HasForeignKey(e => e.DOCSERVER);

            modelBuilder.Entity<DOCSERVER>()
                .HasMany(e => e.INDEX_COLLECTION)
                .WithRequired(e => e.DOCSERVER1)
                .HasForeignKey(e => e.DOCSERVER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DOCSUBCLASS>()
                .Property(e => e.INDEXABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSUBCLASS>()
                .Property(e => e.SHADOW)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSUBCLASS>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCSUBCLASS>()
                .HasMany(e => e.DOCMASTERs)
                .WithOptional(e => e.DOCSUBCLASS)
                .HasForeignKey(e => new { e.C_ALIAS, e.SUBCLASS_ALIAS });

            modelBuilder.Entity<DOCTYPE>()
                .Property(e => e.AUTODETECT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCTYPE>()
                .Property(e => e.INDEXABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCTYPE>()
                .Property(e => e.IS_HIPAA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCTYPE>()
                .HasMany(e => e.APPS)
                .WithRequired(e => e.DOCTYPE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DOCTYPE>()
                .HasMany(e => e.TYPEMAPs)
                .WithOptional(e => e.DOCTYPE)
                .HasForeignKey(e => e.TYPEALIAS);

            modelBuilder.Entity<DOCUSER>()
                .Property(e => e.LOGIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCUSER>()
                .Property(e => e.PWD_NEVER_EXPIRE_F)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCUSER>()
                .Property(e => e.ISEXTERNAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DOCUSER>()
                .Property(e => e.SECUREDOCSERVER)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUSER>()
                .HasMany(e => e.EM_PROJECTS)
                .WithOptional(e => e.DOCUSER)
                .HasForeignKey(e => e.OPERATOR);

            modelBuilder.Entity<DOCUSER>()
                .HasMany(e => e.EM_REQUESTS)
                .WithOptional(e => e.DOCUSER)
                .HasForeignKey(e => e.OPERATOR);

            modelBuilder.Entity<EM_PROJECTS>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMAIL_ATTRS>()
                .Property(e => e.ORIGIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMAIL_ATTRS>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMAIL_ATTRS>()
                .Property(e => e.TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FORM_TEMPLATE>()
                .Property(e => e.DELETEABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FORM_TYPES>()
                .HasMany(e => e.PALETTEs)
                .WithRequired(e => e.FORM_TYPES)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FORM_TYPES>()
                .HasMany(e => e.TEMPLATE_CONTROLS)
                .WithRequired(e => e.FORM_TYPES)
                .HasForeignKey(e => e.TEMPLATE_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FORM>()
                .Property(e => e.DELETEABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FORM>()
                .HasMany(e => e.FORM_CONTROLS)
                .WithRequired(e => e.FORM)
                .HasForeignKey(e => new { e.FORM_ID, e.LOCALE })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GROUP>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GROUP>()
                .Property(e => e.ISEXTERNAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INDEX_COLLECTION>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INDEX_COLLECTION>()
                .HasMany(e => e.DOC_INDEX)
                .WithRequired(e => e.INDEX_COLLECTION)
                .HasForeignKey(e => e.INDX_CLCTN_SID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KEYWORD>()
                .HasMany(e => e.DOC_KEYWORDS)
                .WithRequired(e => e.KEYWORD)
                .HasForeignKey(e => e.KW_SID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KMTAG>()
                .Property(e => e.DEFAULT_SECURITY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJ_ACCESS>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJ_DENIAL>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT_ITEMS>()
                .Property(e => e.ITEMTYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT_ITEMS>()
                .Property(e => e.REFERENCE_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT_ITEMS>()
                .HasMany(e => e.PROJECT_ITEMS1)
                .WithOptional(e => e.PROJECT_ITEMS2)
                .HasForeignKey(e => e.PARENT_SID);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.DEFAULT_SECURITY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.IS_SECURED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.PRJ_STATE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.PRJ_PUBLIC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.INHERITS_SECURITY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.IS_EXTERNAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.EXTRNL_AS_NRML)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.IS_DOC_SVD_SRCH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.IS_PRJ_SVD_SRCH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .Property(e => e.IS_HIDDEN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PROJECT>()
                .HasMany(e => e.EM_PROJECTS)
                .WithRequired(e => e.PROJECT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PROJECT>()
                .HasMany(e => e.EM_REQUESTS)
                .WithRequired(e => e.PROJECT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PROJECT>()
                .HasMany(e => e.PROJECT_NVPS)
                .WithRequired(e => e.PROJECT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PROJECT>()
                .HasMany(e => e.PROJECTS1)
                .WithOptional(e => e.PROJECT1)
                .HasForeignKey(e => e.PRJ_PID);

            modelBuilder.Entity<REEVENTDEF>()
                .Property(e => e.ENABLE_F)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<REEVENTDEF>()
                .HasMany(e => e.REEVENTLOGs)
                .WithRequired(e => e.REEVENTDEF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RERULEHANDLER>()
                .Property(e => e.ENABLED_F)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RERULEHANDLER>()
                .HasMany(e => e.RERULES)
                .WithRequired(e => e.RERULEHANDLER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RERULE>()
                .Property(e => e.ENABLED_F)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RERULE>()
                .HasMany(e => e.REEVENTDEFs)
                .WithMany(e => e.RERULES)
                .Map(m => m.ToTable("RE_RULE_EVENT").MapLeftKey("RULE_RSID").MapRightKey("EVENT_T"));

            modelBuilder.Entity<ROLE_PROFILES>()
                .Property(e => e.SRCH_VALUE_ACCESS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ROLE_PROFILES>()
                .Property(e => e.SET_VALUE_ACCESS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.ROLE_NVPS)
                .WithRequired(e => e.ROLE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.ROLE_PROFILES)
                .WithRequired(e => e.ROLE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.DOCUSERS)
                .WithMany(e => e.ROLES)
                .Map(m => m.ToTable("USERROLES", "MHGROUP").MapLeftKey("ALIAS").MapRightKey("USERID"));

            modelBuilder.Entity<SEARCH_PROFILES>()
                .Property(e => e.IS_SECURED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SECURITY_TEMPLATE>()
                .Property(e => e.DEFAULT_SECURITY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SECURITY_TEMPLATE>()
                .HasMany(e => e.GRP_SCTY_ACC_TEMP)
                .WithRequired(e => e.SECURITY_TEMPLATE)
                .HasForeignKey(e => e.SCTY_TEMP_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SECURITY_TEMPLATE>()
                .HasMany(e => e.USR_SCTY_ACC_TEMP)
                .WithRequired(e => e.SECURITY_TEMPLATE)
                .HasForeignKey(e => e.SCTY_TEMP_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SECURITY_TEMPLATE>()
                .HasMany(e => e.USR_SCTY_TEMP_ASSC)
                .WithRequired(e => e.SECURITY_TEMPLATE)
                .HasForeignKey(e => e.SCTY_TEMP_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SID_TABLE>()
                .Property(e => e.TABLE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SRCH_PROF_ACCESS>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SRCH_PROF_DENIAL>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYNC_ITEMS>()
                .Property(e => e.ITEMTYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYNC_ITEMS>()
                .HasMany(e => e.SYNC_LOCATIONS)
                .WithRequired(e => e.SYNC_ITEMS)
                .HasForeignKey(e => e.SYNC_ITEM_SID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYSTEM_PREFERENCES>()
                .Property(e => e.IS_ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYSTEM_PREFERENCES>()
                .HasMany(e => e.USER_PREFERENCES)
                .WithRequired(e => e.SYSTEM_PREFERENCES)
                .HasForeignKey(e => e.SYSPREFS_SID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_PREFERENCES>()
                .Property(e => e.IS_ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<USERACTION>()
                .Property(e => e.ENABLED)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<USERACTION>()
                .HasMany(e => e.USERHISTORies)
                .WithRequired(e => e.USERACTION)
                .HasForeignKey(e => e.ACTIONSID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USR_SCTY_TEMP_ASSC>()
                .Property(e => e.DEFAULT_VALUE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<WEB_PAGE_ACCESS>()
                .Property(e => e.OBJECT_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CACHE_UPDATE>()
                .Property(e => e.TABLE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CACHE_UPDATE>()
                .Property(e => e.OPER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RESTORETBL>()
                .Property(e => e.ALLVERSIONS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYSTEM_MANAGEMENT>()
                .Property(e => e.SCHEMA_VERSION)
                .IsUnicode(false);

            modelBuilder.Entity<TYPEMAP>()
                .Property(e => e.LEGACYPRIMARYDOCTYPE)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
